using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenimientoController : Controller
    {
        // GET: Mantenimiento
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }

        #region CATEGORIA

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oList = new List<Categoria>();
            oList = new CN_Categoria().Listar();
            return Json(new { data = oList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria obj)
        {
            object resul;

            string mensaje = string.Empty;

            if (obj.IdCategoria == 0)
            {
                //Registrando un nuevo usuario
                resul = new CN_Categoria().Registrar(obj, out mensaje);
            }
            else
            {
                resul = new CN_Categoria().Editar(obj, out mensaje);
            }


            return Json(new { resultado = resul, mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {

            bool resul = false;
            string mensaje = string.Empty;
            resul = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = resul, mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region MARCA
        [HttpGet]
        public JsonResult ListarMarcas()
        {
            List<Marca> oList = new List<Marca>();
            oList = new CN_Marca().Listar();
            return Json(new { data = oList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca obj)
        {
            object resul;

            string mensaje = string.Empty;

            if (obj.IdMarca == 0)
            {
                //Registrando un nuevo usuario
                resul = new CN_Marca().Registrar(obj, out mensaje);
            }
            else
            {
                resul = new CN_Marca().Editar(obj, out mensaje);
            }


            return Json(new { resultado = resul, mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {

            bool resul = false;
            string mensaje = string.Empty;
            resul = new CN_Marca().Eliminar(id, out mensaje);

            return Json(new { resultado = resul, mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region PRODUCTO
        [HttpGet]
        public JsonResult ListarProductos()
        {
           //Crea una lista de productos vacia
            List<Producto> oList = new List<Producto>();
            //llama al metodo listar() de la clase cnproducto que devuelve una lista de objetos 'Producto'. La lista devuelta se le asigna a la lista vacia
            oList = new CN_Producto().Listar();
            //Convierte la lista de objetos a un JSON, "data" es la matriz de la lista. La propiedad JsonRequestBehavior.AllowGet permite que la solicitud HTTP sea una solicitud GET
            return Json(new { data = oList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string obj, /*Aca obtenemos la imagen del producto =>*/ HttpPostedFileBase archivoImage)
        {
            string mensaje = string.Empty;
            //Para guardar una imagen
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            //ACA CONVIERTO el "obj", que esta en modo texto, en un objeto producto
            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(obj);

            decimal precio;

            //Trato de convertir el valor de PrecioTexto a un formato decimal correcto
            if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }

            if (oProducto.IdProducto == 0)
            {
                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);
                if (idProductoGenerado !=0)
                {
                    oProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }


            if (operacion_exitosa)
            {
                if (archivoImage != null)
                {
                    //aca guardamos la ruta del webConfig
                    string ruta_guardar = ConfigurationManager.AppSettings["servidoFotos"];
                    //Aca vemos la extension de la img
                    string extension = Path.GetExtension(archivoImage.FileName);
                    //Aca guardamos el nombre de la imagen, haciendo una concatenacion con el id del producto y la extension
                    string nombre_Image = string.Concat(oProducto.IdProducto.ToString(), extension);



                    try
                    {
                        //Estoy guardando el archivoImages en un path, el path estara, en la ruta que especificamos en la webConfig con el Nombre de la imagen
                        archivoImage.SaveAs(Path.Combine(ruta_guardar, nombre_Image));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }

                    if (guardar_imagen_exito)
                    {
                        oProducto.RutaImg = ruta_guardar;
                        oProducto.NombreImg = nombre_Image;
                        bool rspa = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto, pero hubo problemas con la imagen";
                    }


                }
            }


            return Json(new { operacionExitosa = operacion_exitosa, idGenerado = oProducto.IdProducto, mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oProducto = new CN_Producto().Listar().Where(P => P.IdProducto == id).FirstOrDefault();
            
            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImg, oProducto.NombreImg), out conversion);


            return Json(new
            {
                conversion,
                textoBase64,
                extension = Path.GetExtension(oProducto.NombreImg)
                //Confundido con AllowGet
            },JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {

            bool resul = false;
            string mensaje = string.Empty;
            resul = new CN_Producto().Eliminar(id, out mensaje);

            return Json(new { resultado = resul, mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}