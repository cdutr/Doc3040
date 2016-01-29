//
//  Copyright 2016 Gustavo J Knuppe (https://github.com/knuppe)
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
//    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
//    - May you do good and not evil.                                         -
//    - May you find forgiveness for yourself and forgive others.             -
//    - May you share freely, never taking more than you give.                -
//    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
//
using System;
using System.Xml;

namespace Doc3040.Bacen {

    /// <summary>
    /// Descreve uma operação de crédito dentro do arquivo 3040.
    /// </summary>
    public sealed class Operacao : Elemento {

        internal static readonly string Tag = "Op";

        internal Operacao (Cliente cliente, Doc3040Reader reader) : base(reader) {

            string attr;
            //DateTime dt;

            // Detalhamento do cliente
            if (Attributo ("DetCli", out attr)) {

                if (cliente.TipoPessoa.In (TipoPessoa.PessoaFisica, TipoPessoa.PessoaFisicaNoExterior, TipoPessoa.PessoaFisicaSemCpf)) {
                    AdicionaCritica (TiposCritica.Erro, "O detalhamento do cliente (Op->DetCli) não deve ser informado para clientes pessoa física.");
                } else {

                    if (attr.Length != 14) {
                        AdicionaCritica (TiposCritica.Erro, "O detalhamento do cliente (Op->DetCli) deve conter os 14 digitos do CNPJ.");
                    } else {
                        DetalhamentoCliente = DetalhamentoCliente;
                    }
                }
            }

            // Código do contrato
            if (Attributo ("Contrt", out attr)) {
                Contrato = attr;
                if (reader.ContratoDuplicado (Contrato)) {
                    AdicionaCritica (TiposCritica.Aviso, "O contrato [" + Contrato + "] está duplicado no arquivo.");
                }
            } else {
                AdicionaCritica (TiposCritica.Erro, "O atributo obrigatório de contrato (Op->Contrt) não está definido.");
            }
        }

        /// <summary>
        /// Detalhamento de CNPJ com 14 dígitos
        /// </summary>
        public string DetalhamentoCliente {get; set;}

        /// <summary>
        /// Código de identificação do contrato. Código alfanumérico interno da IF identificador do contrato.
        /// </summary>
        public string Contrato { get; set; }

        /// <summary>
        /// Código identificador da modalidade da operação.
        /// </summary>
        public string Modalidade { get ; set ; }

        /// <summary>
        /// Quantidade de parcelas previstas no contrato. As parcelas devem ser consideradas de forma individual. Podendo ser maior, igual ou menor que o número de meses do contrato.
        /// </summary>
        public int Parcelas {get; internal set;}

    }
}

