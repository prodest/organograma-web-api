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

        UnidadeModeloNegocio Pesquisar(int id);

        UnidadeModeloNegocio Inserir(UnidadeModeloNegocio esferaOrganizacao);

        void Alterar(int id, UnidadeModeloNegocio esferaOrganizacao);

        void Excluir(int id);
    }
}