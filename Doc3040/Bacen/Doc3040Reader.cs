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
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace Doc3040.Bacen {
    using Utility;

    internal class Doc3040Reader : Disposable {

        internal readonly XmlTextReader xml;

        private Dictionary<string, int> contratos;


        public Doc3040Reader (string xmlFilename) {

            if (!File.Exists (xmlFilename))
                throw new FileNotFoundException ("O arquivo XML 3040 nao existe!", xmlFilename);

            xml = new XmlTextReader (xmlFilename);

            if (!xml.Read () || xml.NodeType != XmlNodeType.XmlDeclaration)
                throw new InvalidOperationException ("O arquivo nao contem a declaracao xml ou eh invalido!");

            if (!xml.ReadUntil(XmlNodeType.Element, "Doc3040"))
                throw new InvalidOperationException ("O arquivo XML não contem o elemento root esperado!");

            ValidarContratosDuplicados = true;
        }

        /// <summary>
        /// Determina se o leitor do documento irá validar códigos de contratos duplicados no arquivo.
        /// </summary>
        /// <value><c>true</c> se for validar contratos duplicados; caso contrário, <c>false</c>.</value>
        public bool ValidarContratosDuplicados { get; set; }


        /// <summary>
        /// Efetua a leitura do próximo cliente do documento.
        /// </summary>
        public Cliente Read() {

            // Verifica se não está na tag de cliente na posição atual.
            if (xml.NodeType == XmlNodeType.Element && xml.Name == Cliente.Tag) {
                return new Cliente (this);
            }

            if (xml.ReadUntil (XmlNodeType.Element, Cliente.Tag)) {
                return new Cliente (this);
            }
            return null;
        }


        /// <summary>
        /// Adiciona o contrato ao dicionário e valida se o contrato é duplicado.
        /// </summary>
        /// <param name="contrato">Código do contrato.</param>
        internal bool ContratoDuplicado(string contrato) {
            if (!ValidarContratosDuplicados)
                return false;

            if (contratos == null) {
                contratos = new Dictionary<string, int> {
                    { contrato, 1 }
                };
                return false;
            }

            if (contratos.ContainsKey (contrato)) {
                contrato [contrato]++;
                return true;
            }

            contratos.Add (contrato, 1);
            return false;
        }

    }
}

