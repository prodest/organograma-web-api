﻿using Organograma.Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organograma.Negocio.Base
{
    public interface IUnidadeNegocio
    {
        List<UnidadeModeloNegocio> Listar();

        UnidadeModeloNegocio Pesquisar(string guid);

        UnidadeModeloNegocio Inserir(UnidadeModeloNegocio esferaOrganizacao);

        void Alterar(string guid, UnidadeModeloNegocio esferaOrganizacao);

        void Excluir(string guid);

        List<UnidadeModeloNegocio> PesquisarPorOrganizacao(string guidOrganizacao);

        Task<UnidadeModeloNegocio.Responsavel> PesquisarResponsavel(string guid);
    }
}
