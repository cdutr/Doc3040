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
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace Doc3040.Bacen {
    public sealed class Cliente : Elemento {

        internal static readonly string Tag = "Cli";

        private static readonly DateTime minIniRelactCli;
        private static readonly DateTime maxIniRelactCli;

        static Cliente() {
            minIniRelactCli = new DateTime (1900, 12, 31);
            maxIniRelactCli = new DateTime (2050, 1, 1);
        }

        internal Cliente (Doc3040Reader reader) : base(reader) {

            int i;
            string attr;

            // Tipo pessoa
            if (Attributo("Tp", out attr) ) {

                if (attr.Length != 1) {
                    AdicionaCritica (TiposCritica.Erro, "O atributo obrigatório de tipo de pessoa (Cli->Tp) deve conter um caractere.");
                }

                if (!int.TryParse(attr, out i)) {
                    AdicionaCritica (TiposCritica.Erro, "O atributo obrigatório de tipo de pessoa (Cli->Tp) deve ser numérico.");
                }

                if (i > 0 && i <= 6) {
                    TipoPessoa = (TipoPessoa)i;
                } else {
                    AdicionaCritica (TiposCritica.Erro, "O atributo obrigatório de tipo de pessoa (Cli->Tp) esta fora dos valores validos.");
                }
            } else {
                AdicionaCritica (TiposCritica.Erro, "O atributo obrigatório de tipo de pessoa (Cli->Tp) não está definido.");
            }

            // Código do cliente
            if (Attributo("Cd", out attr)) {

                // TODO: Adicionar validação de CPF/Cnpj

                CodigoCliente = attr;

                if (CodigoCliente.Length > 14) {
                    AdicionaCritica (TiposCritica.Erro, "O código do cliente (Cli->Cd) contém mais de 14 caracteres.");
                }

                if (CodigoCliente.Trim ().Length == 0) {
                    AdicionaCritica (TiposCritica.Erro, "O código do cliente (Cli->Cd) está vazio.");
                }

            } else {
                AdicionaCritica (TiposCritica.Erro, "O atributo obrigatório do código do cliente (Cli->Cd) não está definido.");
            }


            // Autorização SCR
            if (Attributo ("Autorzc", out attr)) {

                if (attr.Length != 1) {
                    AdicionaCritica (TiposCritica.Erro, "O atributo obrigatório de autorização (Cli->Autorzc) deve conter apenas um caractere.");
                }

                Autorizacao = attr.Trim ().ToUpperInvariant () == "S";

            } else {
                AdicionaCritica (TiposCritica.Erro, "O atributo obrigatório de autorização (Cli->Autorzc) não está definido.");
            }

            // Porte cliente
            if (Attributo ("PorteCli", out attr)) {
                if (!int.TryParse (attr, out i)) {
                    AdicionaCritica (TiposCritica.Erro, "O atributo obrigatório de autorização (Cli->Autorzc) não é numérico.");
                }

                PorteCli = i;
            } else {
                AdicionaCritica (TiposCritica.Erro, "O atributo obrigatório de porte de cliente (Cli->PorteCli) não está definido.");
            }


            // IniRelactCli
            if (Attributo ("IniRelactCli", out attr)) {
                DateTime dt;

                attr = Attributos ["IniRelactCli"];
                if (DateTime.TryParseExact (attr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt)) {

                    if (dt <= minIniRelactCli || dt >= maxIniRelactCli) {
                        AdicionaCritica (TiposCritica.Erro, "O valor do atributo de inicio de relacionamento (Cli->IniRelactCli) esta fora dos limites válidos.");
                    } else {
                        InicioRelacionamento = dt;
                    }
                } else {
                    AdicionaCritica (TiposCritica.Erro, "O valor do atributo de inicio de relacionamento (Cli->IniRelactCli) é inválido.");
                }
            } else {
                AdicionaCritica (TiposCritica.Erro, "O atributo obrigatório de inicio de relacionamento (Cli->IniRelactCli) não está definido.");
            }


            // Classificação de risco do cliente
            if (Attributo ("ClassCli", out attr)) {

                attr = Attributos ["ClassCli"];

                if (attr.In ("AA", "A", "B", "C", "D", "E", "F", "G", "H")) {
                    RiscoCliente = attr;
                } else {
                    AdicionaCritica (TiposCritica.Erro, "A classificação de risco do cliente (Cli->ClassCli) é inválida.");
                }
            } else {
                AdicionaCritica (TiposCritica.Erro, "O atributo obrigatório da classificação do cliente (Cli->ClassCli) não está definido.");
            }

            while (reader.xml.ReadUntil(XmlNodeType.Element)) {
                switch (reader.xml.Name) {
                case "Op":
                    Operacoes.Add (new Operacao (this, reader));
                    break;
                case "Venc":
                case "Gar":
                case "Inf":
                    throw new NotImplementedException ();                   
                case "Cli":
                    // encontramos a próxima tag de cliente.
                    return;
                default:
                    AdicionaCritica (TiposCritica.Erro, "Uma tag não esperada foi encontrada no arquivo.");
                    return;
                }
            }
        }

        /// <summary>
        /// Determina o tipo de pessoa.
        /// </summary>
        public TipoPessoa TipoPessoa { get; private set; }

        /// <summary>
        /// Identificação do cliente (CPF, CNPJ  de 8 dígitos ou outro código que defina o cliente caso não seja PF ou PJ) 
        /// </summary>
        public string CodigoCliente {get; private set; }

        /// <summary>
        /// Autorização para a consulta das informações do cliente no SCR.
        /// </summary>
        public bool Autorizacao {get; private set;}

        /// <summary>
        /// Determina o porte do cliente.
        /// </summary>
        public int PorteCli {get; private set; }

        /// <summary>
        /// Data de abertura de conta corrente ou outra data considerada relevante para avaliação do risco do crédito.
        /// </summary>
        public DateTime InicioRelacionamento {get; private set;}

        /// <summary>
        /// Classificação de risco do cliente.
        /// </summary>
        public string RiscoCliente { get; private set; }

        /// <summary>
        /// Retorna a lista de operações do cliente.
        /// </summary>
        public List<Operacao> Operacoes { get; private set; }
    }
}

