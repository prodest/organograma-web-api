﻿using Organograma.Infraestrutura.Comum;
using Organograma.Negocio.Modelos;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Linq;

namespace Organograma.Negocio.Validacao
{
    //TODO: Implemetar esta classe
    public class EmailValidacao
    {
        private string padraoEmail = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

        internal void Preenchido(List<EmailModeloNegocio> emails)
        {
            if (emails != null)
            {
                foreach (var email in emails)
                {
                    Preenchido(email);
                }
            }
        }

        internal void Valido(List<EmailModeloNegocio> emails)
        {
            if (emails != null)
            {
                foreach (var email in emails)
                {
                    Valido(email);
                }

                Repetido(emails);
            }
        }

        internal void Preenchido(EmailModeloNegocio email)
        {
            if (email != null)
            {
                if (string.IsNullOrEmpty(email.Endereco))
                {
                    throw new OrganogramaRequisicaoInvalidaException("Endereço do email não preenchido");
                }
            }
        }

        internal void Valido(EmailModeloNegocio email)
        {
            if (email != null)
            {
                Regex emailRegex = new Regex(padraoEmail);

                if (!emailRegex.IsMatch(email.Endereco))
                {
                    throw new OrganogramaRequisicaoInvalidaException("Email \"" + email.Endereco + "\" inválido.");
                }
            }
        }

        private void Repetido(List<EmailModeloNegocio> emails)
        {
            var duplicados = emails.GroupBy(e => e.Endereco)
                                   .Where(g => g.Count() > 1)
                                   .Select(g => g.Key)
                                   .ToList(); ;

            if (duplicados != null && duplicados.Count > 0)
                throw new OrganogramaRequisicaoInvalidaException("Existe email duplicado.");
        }
    }
}
