using System.Diagnostics;
using WorkerServicePostgres.Entities;
using WorkerServicePostgres.Repositories;

namespace WorkerServicePostgres.Services;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRepository<Carro> _carroRepository;


    public Worker(ILogger<Worker> logger, IRepository<Carro> carroRepository)
    {
        _logger = logger;
        _carroRepository = carroRepository;
        IniciarTimer();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        var guid = Guid.NewGuid();
        for (int i = 0; i < 1000; i++)
        {
            var novoCarro = new Carro
            {
                Id = Guid.NewGuid(),
                Marca = "Toyota",
                Modelo = "Corolla",
                Ano = 2022
            };
            await MessageReceived(novoCarro);
        }

        stopwatch.Stop();
        _logger.LogInformation("Tempo total: " + stopwatch.ElapsedMilliseconds.ToString());
    }

    private const int _batchSize = 400;
    private List<Carro> _carrosParaAdicionar = new List<Carro>();
    private List<Guid> _idsParaDeletar = new List<Guid>();
    private Timer _timer;

    public void IniciarTimer()
    {
        _timer = new Timer(async _ => await FinalizarProcessamento(), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
    }

    private async Task MessageReceived(Carro carro)
    {
            AdicionarCarroNoLote(_carrosParaAdicionar, _idsParaDeletar);

            if (_carrosParaAdicionar.Count == _batchSize)
            {
                await ProcessarLoteAsync(_carrosParaAdicionar, _idsParaDeletar);
                _logger.LogInformation($"Gravando lote de {_batchSize} carros: Interação:");
            }
    }

    public async Task FinalizarProcessamento()
    {
        _logger.LogInformation($"Gravando o restante dos carros: {_carrosParaAdicionar.Count} carros.");
        if (_carrosParaAdicionar.Count > 0)
        {
            await ProcessarLoteAsync(_carrosParaAdicionar, _idsParaDeletar);
            _carrosParaAdicionar.Clear();
            _idsParaDeletar.Clear();
        }
    }

    private void AdicionarCarroNoLote(List<Carro> carrosParaAdicionar, List<Guid> idsParaDeletar)
    {
        var novoCarro = new Carro
        {
            Id = Guid.NewGuid(),
            Marca = "Toyota",
            Modelo = "Corolla",
            Ano = 2022
        };

        idsParaDeletar.Add(novoCarro.Id);
        carrosParaAdicionar.Add(novoCarro);
    }

    private async Task ProcessarLoteAsync(List<Carro> carrosParaAdicionar, List<Guid> idsParaDeletar)
    {
        await _carroRepository.DeleteRangeAsync(idsParaDeletar);
        await _carroRepository.AddRangeAsync(carrosParaAdicionar);
        idsParaDeletar.Clear();
        carrosParaAdicionar.Clear();

    }
}
