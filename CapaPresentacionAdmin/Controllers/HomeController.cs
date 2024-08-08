using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CapaPresentacionAdmin.Controllers
{
    //Nos permite comunicar con nuestra vista
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oList = new List<Usuario>();
            oList = new CN_Usuarios().Listar();
            return Json(new { data = oList }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarUsuario(Usuario obj)
        {
            object resul;

            string mensaje = string.Empty;

            if (obj.IdUsuario == 0)
            {
                //Registrando un nuevo usuario
                resul = new CN_Usuarios().Registrar(obj, out mensaje);
            }
            else
            {
                resul = new CN_Usuarios().Editar(obj, out mensaje);
            }


            return Json(new { resultado = resul, mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool resul = false;
            string mensaje = string.Empty;
            resul = new CN_Usuarios().Eliminar(id, out mensaje);

            return Json(new { resultado = resul, mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult VistaDashBoard()
        {
            Dashboard objeto = new CN_Reporte().VerDashBoard();
            return Json(new { resultado = objeto}, JsonRequestBehavior.AllowGet);
        }
    }
}