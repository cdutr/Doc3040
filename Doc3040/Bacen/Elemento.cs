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
using System.Collections.Generic;

namespace Doc3040.Bacen {
    public abstract class Elemento {

        /// <summary>
        /// Retorna os atributos do elemento.
        /// </summary>
        protected Dictionary<string, string> Attributos { get; private set; }

        /// <summary>
        /// Retorna o nome do elemento.
        /// </summary>
        protected string Nome { get; private set; }


        /// <summary>
        /// Lista as criticas do elemento.
        /// </summary>
        public List<Critica> Criticas { get; private set; }


        internal Elemento (Doc3040Reader reader) {           

            Nome = reader.xml.Name;
            Attributos = new Dictionary<string, string> (reader.xml.AttributeCount);
            while (reader.xml.MoveToNextAttribute ())
                Attributos [reader.xml.Name] = reader.xml.Value;

        }

        /// <summary>
        /// Procura e retorna o valor de um atributo obrigatório.
        /// </summary>
        /// <param name="nome">Nome do atributo.</param>
        /// <param name="valor">Valor do atributo.</param>
        /// <returns><c>true</c> se o atributo existir.</returns>
        protected bool Attributo(string nome, out string valor) {
            if (Attributos != null && Attributos.ContainsKey (nome)) {
                valor = Attributos [nome];
                return true;
            }
            valor = null;
            return false;
        }


        /// <summary>
        /// Adiciona uma critica ao elemento.
        /// </summary>
        /// <param name="tipo">Tipo da critica</param>
        /// <param name="mensagem">Mensagem associada a critica.</param>
        protected void AdicionaCritica(TiposCritica tipo, string mensagem) {
            
            Criticas.Add (new Critica {
                Tipo = tipo,
                Mensagem = mensagem
            });

        }
    }
}