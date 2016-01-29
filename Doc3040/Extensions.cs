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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Doc3040 {
    internal static class Extensions {


        public static bool ReadUntil(this XmlTextReader reader, XmlNodeType type, string nome = null) {           
            while (reader.Read ()) {
                if (reader.NodeType == type && (nome == null || reader.Name == nome)) 
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Determines if the instance is equal to any of the specified values
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="input">The input object.</param>
        /// <param name="values">The values to be compared.</param>
        /// <returns><c>true</c> if the instance is equal to any of the specified values, <c>false</c> otherwise.</returns>
        public static bool In<T>(this T input, params T[] values) {
            foreach (var value in values) {
                if (ReferenceEquals(value, input))
                    return true;

                if (value.Equals(input))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the instance is equal to any of the specified values
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="input">The input object.</param>
        /// <param name="values">The values to be compared.</param>
        /// <returns><c>true</c> if the instance is equal to any of the specified values, <c>false</c> otherwise.</returns>
        public static bool In<T>(this T input, IEnumerable<T> values) {
            return values.Any(arg => input.Equals(arg));
        }
    }
}

