using System;

namespace Doc3040 {
	public class MainClass {
		public static void Main (string[] args) {
			
            using (var reader = new Doc3040.Bacen.Doc3040Reader ("../../../Bacen/exemploDocPadraoInfosBasicas.xml")) {

                var cli = reader.Read ();

                Console.WriteLine ("Cliente");

            }

		}
	}
}
