using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FirebaseStorage_Practica.Controllers
{
    public class FirebaseStorageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]

        public async Task<ActionResult> SubirArchivo(IFormFile archivo)

        {


            //Leemos el archivo subido.
            Stream archivoASubir = archivo.OpenReadStream();

            //Configuramos la conexion hacia FireBase
            string email = "melvin.mayen@catolica.edu.sv"; // Correo para autenticar en FireBase
                                                           // Contrasena establaecida en la autenticar en FireBase
            string ruta = "practica-razor-html-helper.appspot.com"; // URL donde se guardaran los archivo.


            string clave = "12345real";

            string api_key = "AIzaSyAPCODoriuNPCsM_P4MCAtV4dLkIqwAdk4";


            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFireBase.FirebaseToken;

            var tareaCargarArchvio = new FirebaseStorage(ruta,
                            new FirebaseStorageOptions

                            {
                                AuthTokenAsyncFactory = () => Task.FromResult(tokenUser),
                                ThrowOnCancel = true

                            }


            ).Child("Archivos")
            .Child(archivo.FileName)
            .PutAsync(archivoASubir, cancellation.Token);

            var urlArchivoCargado = await tareaCargarArchvio;



            return RedirectToAction("VerImagen");

        }


    }
}
