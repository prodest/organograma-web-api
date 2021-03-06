﻿using Organograma.Dominio.Base;
using Organograma.Dominio.Modelos;
using Organograma.Negocio.Modelos;
using Organograma.Infraestrutura.Comum;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Organograma.Negocio.Validacao
{
    public class ContatoValidacao
    {
        private IRepositorioGenerico<Contato> repositorioContatos;
        private IRepositorioGenerico<TipoContato> repositorioTiposContato;

        public ContatoValidacao(IRepositorioGenerico<Contato> repositorioContatos, IRepositorioGenerico<TipoContato> repositorioTiposContato)
        {
            this.repositorioContatos = repositorioContatos;
            this.repositorioTiposContato = repositorioTiposContato;
        }

        internal void NaoEncontrado(ContatoModeloNegocio contato)
        {
            if (repositorioContatos.Where(c => c.Id == contato.Id).SingleOrDefault() == null)
            {
                throw new OrganogramaNaoEncontradoException("Contato não encontrado");
            }
        }

        internal void Preenchido(List<ContatoModeloNegocio> contatos)
        {
            if (contatos != null)
            {
                foreach (var contato in contatos)
                {
                    Preenchido(contato);
                }
            }
        }

        internal void Valido(List<ContatoModeloNegocio> contatos)
        {
            if (contatos != null)
            {
                foreach (var contato in contatos)
                {
                    Valido(contato);
                }

                Repetido(contatos);
            }
        }


        internal void Preenchido(ContatoModeloNegocio contato)
        {
            if (contato != null)
            {
                TelefonePreenchido(contato);
                TipoContatoPreenchido(contato);
            }
        }

        internal void Valido(ContatoModeloNegocio contato)
        {
            if (contato != null)
            {
                TipoContatoExiste(contato);
                TelefoneValido(contato);
            }
        }

        internal void TelefonePreenchido(ContatoModeloNegocio contato)
        {
            if (string.IsNullOrEmpty(contato.Telefone))
            {
                throw new OrganogramaRequisicaoInvalidaException("Telefone não preenchido.");
            }
        }

        internal void TipoContatoPreenchido(ContatoModeloNegocio contato)
        {
            if (contato.TipoContato.Id <= 0)
            {
                throw new OrganogramaRequisicaoInvalidaException("Tipo do Contato não informado.");
            }
        }

        private void TelefoneValido(ContatoModeloNegocio contato)
        {
            ApenasNumeros(contato);
            NumeroDigitosValido(contato);
        }

        private void ApenasNumeros(ContatoModeloNegocio contato)
        {
            try
            {
                long.Parse(contato.Telefone);
            }
            catch (Exception)
            {
                throw new OrganogramaRequisicaoInvalidaException("O Telefone deve possuir apenas números.");
            }
        }

        private void NumeroDigitosValido (ContatoModeloNegocio contato)
        {
            TipoContato tipoContato = repositorioTiposContato.Where(t => t.Id == contato.TipoContato.Id).Single();
            if (contato.Telefone.Length != tipoContato.QuantidadeDigitos)
            {
                throw new OrganogramaRequisicaoInvalidaException("Telefone do tipo " + tipoContato.Descricao + " devem possuir " + tipoContato.QuantidadeDigitos + " dígitos.");
            }
        }

        internal void TipoContatoExiste(ContatoModeloNegocio contato)
        {
            TipoContato tipoContato = repositorioTiposContato.Where(t => t.Id == contato.TipoContato.Id).SingleOrDefault();

            if (tipoContato == null)
            {
                throw new OrganogramaRequisicaoInvalidaException("Tipo do Contato não existe.");
            }
        }

        private void Repetido(List<ContatoModeloNegocio> contatos)
        {
            var duplicados = contatos.GroupBy(e => e.Telefone)
                                   .Where(g => g.Count() > 1)
                                   .Select(g => g.Key)
                                   .ToList(); ;

            if (duplicados != null && duplicados.Count > 0)
                throw new OrganogramaRequisicaoInvalidaException("Existe contato duplicado.");
        }
    }
}