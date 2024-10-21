namespace WorkerServicePostgres.Entities;

public class Carro
{
    public Guid Id { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Ano { get; set; }
}