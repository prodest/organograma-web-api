﻿using Organograma.Dominio.Base;
using Organograma.Dominio.Modelos;
using Organograma.Infraestrutura.Comum;
using Organograma.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Organograma.Negocio.Validacao
{
    public class UnidadeValidacao
    {
        IRepositorioGenerico<Unidade> repositorioUnidades;
        IRepositorioGenerico<TipoUnidade> repositorioTiposUnidades;
        IRepositorioGenerico<Organizacao> repositorioOrganizacoes;

        public UnidadeValidacao(IRepositorioGenerico<Unidade> repositorioUnidades,
                                IRepositorioGenerico<TipoUnidade> repositorioTiposUnidades,
                                IRepositorioGenerico<Organizacao> repositorioOrganizacoes)
        {
            this.repositorioUnidades = repositorioUnidades;
            this.repositorioTiposUnidades = repositorioTiposUnidades;
            this.repositorioOrganizacoes = repositorioOrganizacoes;
        }

        #region Verificação de unidade não nula
        internal void NaoNula(UnidadeModeloNegocio unidade)
        {
            if (unidade == null)
                throw new OrganogramaRequisicaoInvalidaException("Unidade não pode ser nula.");
        }
        #endregion

        #region Verificações de preenchimento de campos obrigatórios
        internal void IdPreenchido(int id)
        {
            if (id == default(int))
                throw new OrganogramaRequisicaoInvalidaException("O id da unidade deve ser preenchido.");
        }

        internal void IdPreenchido(UnidadeModeloNegocio unidade)
        {
            IdPreenchido(unidade.Id);
        }

        internal void IdUnidadePaiPreenchido(UnidadeModeloNegocio unidadePai)
        {
            if (unidadePai.Id == default(int))
                throw new OrganogramaRequisicaoInvalidaException("O id da unidade pai deve ser preenchido.");
        }

        internal void Preenchida(UnidadeModeloNegocio unidade)
        {
            NomePreenchido(unidade.Nome);

            SiglaPreenchida(unidade.Sigla);
        }

        internal void NomePreenchido(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new OrganogramaRequisicaoInvalidaException("O nome da unidade deve ser preenchido.");
        }

        internal void SiglaPreenchida(string sigla)
        {
            if (string.IsNullOrWhiteSpace(sigla))
                throw new OrganogramaRequisicaoInvalidaException("A sigla da unidade deve ser preenchida.");
        }

        internal void PossuiFilho(int id)
        {
            var unidadesFilhas = repositorioUnidades.Where(u => u.IdUnidadePai == id)
                                                    .ToList();

            if (unidadesFilhas != null && unidadesFilhas.Count > 0)
                throw new OrganogramaRequisicaoInvalidaException("A unidade possui unidades filhas.");
        }

        internal void UnidadePaiPreenchida(UnidadeModeloNegocio unidadePai)
        {
            if (unidadePai != null)
                GuidValido(unidadePai.Guid, true);
        }

        #endregion

        #region Validações de negócio
        internal void IdValido(int id)
        {
            if (id <= 0)
                throw new OrganogramaRequisicaoInvalidaException("O id da unidade é inválido.");
        }

        internal void IdValido(UnidadeModeloNegocio unidade)
        {
            IdValido(unidade.Id);
        }

        internal void IdUnidadePaiValido(UnidadeModeloNegocio unidade)
        {
            if (unidade.Id <= 0)
                throw new OrganogramaRequisicaoInvalidaException("O id da unidade pai é inválido.");
        }

        internal void Valida(UnidadeModeloNegocio unidade)
        {
            NomeExiste(unidade);
            SiglaExiste(unidade);
        }

        internal void EnderecoNaoExiste(UnidadeModeloNegocio unidade)
        {
            var endereco = repositorioUnidades.Where(u => u.Id == unidade.Id)
                                              .Select(u => u.Endereco)
                                              .SingleOrDefault();

            if (endereco == null)
                throw new OrganogramaRequisicaoInvalidaException("A unidade não possui endereço.");

        }

        internal void NomeExiste(UnidadeModeloNegocio unidade)
        {
            var uni = repositorioUnidades.Where(u => u.Nome.ToUpper().Equals(unidade.Nome.ToUpper())
                                                  && u.IdOrganizacao == unidade.Organizacao.Id
                                                  && u.Id != unidade.Id)
                                         .SingleOrDefault();

            if (uni != null)
                throw new OrganogramaRequisicaoInvalidaException("Já existe uma unidade com este nome.");
        }

        internal void SiglaExiste(UnidadeModeloNegocio unidade)
        {
            var uni = repositorioUnidades.Where(u => u.Sigla.ToUpper().Equals(unidade.Sigla.ToUpper())
                                                  && u.IdOrganizacao == unidade.Organizacao.Id
                                                  && u.Id != unidade.Id)
                                         .SingleOrDefault();

            if (uni != null)
                throw new OrganogramaRequisicaoInvalidaException("Já existe uma unidade com esta sigla.");
        }

        internal void UnidadePaiValida(UnidadeModeloNegocio unidadePai)
        {
            if (unidadePai != null)
            {
                GuidValido(unidadePai.Guid, true);
                Existe(unidadePai);
            }
        }

        private void Existe(UnidadeModeloNegocio unidade, bool unidadePai = false)
        {
            if (unidade != null)
            {
                Guid guid = new Guid(unidade.Guid);

                var uni = repositorioUnidades.Where(u => u.IdentificadorExterno.Guid.Equals(guid))
                                             .SingleOrDefault();

                if (uni == null)
                    throw new OrganogramaNaoEncontradoException("Unidade " + (unidadePai ? "pai " : "") + "não encontrada.");
            }
        }

        private void UnidadePaiExiste(UnidadeModeloNegocio unidadePai)
        {
            if (unidadePai != null)
            {
                var uniPai = repositorioUnidades.Where(u => u.Id == unidadePai.Id)
                                                .SingleOrDefault();

                if (uniPai == null)
                    throw new OrganogramaNaoEncontradoException("Unidade pai não encontrada.");
            }
        }

        #endregion

        internal void GuidAlteracaoValido(string guid, UnidadeModeloNegocio unidade)
        {
            if (!guid.Equals(unidade.Guid))
                throw new OrganogramaRequisicaoInvalidaException("Identificadores da unidade não podem ser diferentes.");
        }

        internal void IdAlteracaoValido(int id, UnidadeModeloNegocio unidade)
        {
            if (id != unidade.Id)
                throw new OrganogramaRequisicaoInvalidaException("Identificadores da unidade não podem ser diferentes.");
        }

        internal void NaoEncontrado(Unidade unidade)
        {
            if (unidade == null)
                throw new OrganogramaNaoEncontradoException("Unidade não encontrada.");
        }

        internal void NaoEncontrado(List<Unidade> unidades)
        {
            if (unidades == null || unidades.Count <= 0)
                throw new OrganogramaNaoEncontradoException("Unidade não encontrada.");
        }

        internal void GuidValido(string guid, bool unidadePai = false)
        {
            try
            {
                Guid g = new Guid(guid);

                if (g.Equals(Guid.Empty))
                    throw new OrganogramaRequisicaoInvalidaException("Identificador " + (unidadePai ? "da unidade pai " : "") + "inválido.");
            }
            catch (FormatException)
            {
                throw new OrganogramaRequisicaoInvalidaException("Formato do identificador " + (unidadePai ? "da unidade pai " : "") + "inválido.");
            }
        }
    }
}
