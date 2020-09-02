using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using BenchmarkDotNet.Order;
using System.Threading.Tasks;
using Bogus;
using TDSA.Business.Models;
using System.Linq;

namespace TDSA.Perfom
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class FuncoesMatematicas
    {
        [Params(200)]
        public double Number
        {
            get;
            set;
        }

        [Benchmark]
        public double NumeroAoQuadrado()
        {
            return this.Number * this.Number;
        }

        [Benchmark]
        public double NumeroAoQuadradoMathPow()
        {
            Task.Delay(130);

            return Math.Pow(this.Number, 2);
        }

    }

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class TesteService
    {
        [Params(200)]
        public int Number
        {
            get;
            set;
        }

        [Benchmark]
        public async Task SimulandoSemConsultarBanco()
        {
            var faker = new Faker("pt_BR");

            var especialidades = new Faker<Especialidade>("pt_BR").CustomInstantiator((f) => new Especialidade(Guid.NewGuid(), "abcdjshdkskdjs"))
                                                                   .Generate(50);

            foreach (var especialidade in especialidades)
            {
                await Task.Delay(5);
                Number++;
            }

            await Task.Delay(100);

            //Task.CompletedTask;
        }

        [Benchmark]
        public async Task SimulandoConsultandoBanco()
        {
            var faker = new Faker("pt_BR");

            var especialidades = new Faker<Especialidade>("pt_BR").CustomInstantiator((f) => new Especialidade(Guid.NewGuid(), "abcdjshdkskdjs"))
                                                                   .Generate(50);

            var medico = new Faker<Medico>("pt_BR").CustomInstantiator((f) => new Medico(Guid.NewGuid(), "ABC", "abc", "abcds", especialidades))
                                                    .Generate(1);

            foreach (var especialidade in especialidades)
            {
                await Task.Delay(115);
                Number++;
            }

            await Task.Delay(100);

            //return true;
        }

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<TesteService>();
            Console.Read();
        }
    }
}
