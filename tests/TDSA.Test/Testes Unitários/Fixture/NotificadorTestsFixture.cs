using Bogus;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDSA.Business.Notificacoes;
using Xunit;

namespace TDSA.Test.Testes_Unitários.Fixture
{
    [CollectionDefinition(nameof(NotificadorTestsFixtureCollection))]
    public class NotificadorTestsFixtureCollection : ICollectionFixture<NotificadorTestsFixture>
    { }

    public class NotificadorTestsFixture : IDisposable
    {
        public IEnumerable<Notificacao> GerarNotificacoes(int quantidade)
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var notificacoes = new Faker<Notificacao>("pt_BR")
                .CustomInstantiator((f) => new Notificacao(f.Random.String(10, 'a', 'z'),
                                                           f.Random.String(15, 'a', 'z')));

            return notificacoes.Generate(quantidade);
        }

        public List<Notificacao> ObterNotificacoes()
        {
            return GerarNotificacoes(5).ToList();
        }

        public bool TemNotificacao(bool possuiNotificacao)
        {
            if (!possuiNotificacao)
                return possuiNotificacao;

            var notificacoes = GerarNotificacoes(5).ToList();

            return notificacoes.Any();
        }

        public void Dispose()
        {

        }
    }
}
