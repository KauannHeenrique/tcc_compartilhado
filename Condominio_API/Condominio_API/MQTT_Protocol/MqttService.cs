using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;
using System.Text.Json;
using condominio_API.Data;
using condominio_API.Models;

public class MqttService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MqttService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync()
    {
        var factory = new MqttFactory();
        var client = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("broker.hivemq.com", 1883) // ou o IP do seu broker Mosquitto local
            .Build();

        client.UseConnectedHandler(async e =>
        {
            Console.WriteLine("Conectado ao broker MQTT!");
            await client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("condominio/entrada").Build());
        });

        client.UseApplicationMessageReceivedHandler(async e =>
        {
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            Console.WriteLine($"Mensagem recebida via MQTT: {payload}");

            try
            {
                var leitura = JsonSerializer.Deserialize<LeituraDto>(payload);

                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (leitura.Tipo == "qrcode")
                {
                    var visitante = db.Visitantes.FirstOrDefault(v => v.QRCode == leitura.Valor);
                    if (visitante != null)
                    {
                        db.RegistrosVisitantes.Add(new RegistroVisitante
                        {
                            VisitanteId = visitante.Id,
                            DataHora = DateTime.Now
                        });
                        await db.SaveChangesAsync();
                        Console.WriteLine("Acesso visitante registrado.");
                    }
                    else Console.WriteLine("QRCode inválido.");
                }
                else if (leitura.Tipo == "rfid")
                {
                    var morador = db.Usuarios.FirstOrDefault(u => u.CodigoRFID == leitura.Valor);
                    if (morador != null)
                    {
                        db.RegistrosMoradores.Add(new RegistroMorador
                        {
                            MoradorId = morador.UsuarioId,
                            DataHora = DateTime.Now
                        });
                        await db.SaveChangesAsync();
                        Console.WriteLine("Acesso morador registrado.");
                    }
                    else Console.WriteLine("RFID inválido.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
            }
        });

        await client.ConnectAsync(options);
    }
}
