﻿using ClientWCF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientWCF.Controllers
{
    public class OperazioneController : Controller
    {
        // GET: Operazione
        public ActionResult Index()
        {
            return View();
        }

        //recupera tutte le operazioni del DB e le passa alla view
        public ActionResult Operazioni()
        {
            ListaOperazioni LO = new ListaOperazioni();
            if (ModelState.IsValid)
            {
                //connessione col service
                try
                {
                    var wcf = new ServiceReference1.Service1Client();

                    //in listaOpe ho tutte le operazioni
                    var listaOpe = wcf.getOperazioni();

                    if (listaOpe == null)
                    {
                        throw new Exception("Non ci sono Operazioni!");
                    }
                    else
                    {
                        //converto le la lista di operazioni da server a client
                        LO.ConvertServerList(listaOpe);
                        //ritorno alla view con la lista operazioni client
                        return View(LO);
                    }

                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                    return View("Error");
                }
            }

            ViewBag.Message = "Dati non consoni al Model specificato!";
            return View("Error");
        }
    }
}