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

namespace Doc3040 {
    public enum TipoPessoa {
        Desconhecido = 0,
        PessoaFisica = 1,
        PessoaJuridica = 2,
        PessoaFisicaNoExterior = 3,
        PessoaJuridicaNoExterior = 4,
        PessoaFisicaSemCpf = 5,
        PessoaJuridicaSemCnpj = 6
    }

    /// <summary>
    /// Descreve uma critica em um elemento.
    /// </summary>
    public struct Critica {

        /// <summary>
        /// Determina o tipo da critica.
        /// </summary>
        public TiposCritica Tipo { get; internal set; }

        /// <summary>
        /// Retorna a mensagem da critica.
        /// </summary>
        public string Mensagem { get; internal set; }

    }

    /// <summary>
    /// Define os tipos possíveis de criticas.
    /// </summary>
    public enum TiposCritica {

        /// <summary>
        /// Define a critica como um erro critico
        /// </summary>
        /// <remarks>>
        /// Erros criticos indicam que o arquivo não está em conformidade conforme o Bacen.
        /// </remarks>
        Erro = 1,

        /// <summary>
        /// Define o erro como lógico.
        /// </summary>
        /// <remarks>
        /// Erro lógico pode indicar um problema de lógica nos dados do arquivo.
        /// </remarks>
        ErroLogico = 2,


        /// <summary>
        /// Define uma critica como aviso.
        /// </summary>
        Aviso = 3

    }
}