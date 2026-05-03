using System.Collections.Generic;

namespace SistemaRestaurante.Domain.Entities
{
    public class Prato
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        private readonly List<ItemReceita> _receita = new();
        public IReadOnlyCollection<ItemReceita> Receita => _receita.AsReadOnly();

        public Prato(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public void AdicionarItemReceita(ItemReceita item)
        {
            _receita.Add(item);
        }
    }
}