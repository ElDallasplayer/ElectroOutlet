using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Entities.Standard
{
    [Serializable()]
    [DataContract]
    public class Enrolador
    {
        [DataMember]
        public byte[] Template
        { get; set; }

        [DataMember]
        public string ImageAsBytes
        { get; set; }

        [DataMember]
        public EstadosDevueltosPorServicio EstadoDeServicio
        { get; set; }

        [DataMember]
        public CalidadDeHuellaEnum CalidadHuella
        { get; set; }

        [DataMember]
        public string Error
        { get; set; }

        [DataMember]
        public bool ValorComparacion
        { get; set; }
    }

    public enum CalidadDeHuellaEnum
    {
        Error,
        Buena,
        MuyBuena,
        Excelente
    }

    public enum EstadosDevueltosPorServicio
    {
        Error,
        Fault,
        Success
    }
}