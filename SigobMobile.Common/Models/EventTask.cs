namespace SigobMobile.Common.Models
{
    using System;
    using Newtonsoft.Json;

    public class EventTask
    {
        [JsonProperty("tipoMomentoTarea")]
        public EventTaskMoment TaskMomentId { get; set; }

        [JsonProperty("codigo_tarea")]
        public int Id { get; set; }

        [JsonProperty("fecha_tarea")]
        public DateTime EndProgrammedDate { get; set; }

        [JsonProperty("titulo")]
        public string Title { get; set; }

        [JsonProperty("descripcion")]
        public string Description { get; set; }

        [JsonProperty("esExterno")]
        public bool IsExternalResponsible { get; set; }

        [JsonProperty("tipo")]
        public TSigColorTuple Type { get; set; }

        [JsonProperty("tipo_Color_Red")]
        public byte TypeRed { get; set; }

        [JsonProperty("tipo_Color_Green")]
        public byte TypeGreen { get; set; }

        [JsonProperty("tipo_Color_Blue")]
        public byte TypeBlue { get; set; }

        [JsonProperty("puesta_por")]
        public string ProgrammerName { get; set; }

        [JsonProperty("nombre_responsable")]
        public string ResponsibleName { get; set; }

        [JsonProperty("monitor")]
        public string MonitorName { get; set; }

        [JsonProperty("estado")]
        public TStatusTasK Status { get; set; }

        [JsonProperty("estadoDescripcion")]
        public string StatusDescription { get; set; }

        [JsonProperty("reiteraciones")]
        public short Reiterations { get; set; }

        [JsonProperty("propietario")]
        public bool IsOwner { get; set; }

        [JsonProperty("reporte")]
        public string Report { get; set; }

        [JsonProperty("fechaModificacionReporte")]
        public DateTime? LastReportUpdate { get; set; }

        [JsonProperty("fechaProximoReporte")]
        public DateTime? NextReportDate { get; set; }

        [JsonProperty("estadoReporte")]
        public int ReportStatus { get; set; }

        [JsonProperty("periodicidad")]
        public byte Periodicity { get; set; }

        [JsonProperty("reporteRevisado")]
        public byte RevisedReport { get; set; }

        [JsonProperty("despachoMonitor")]
        public string MonitorOfficeId { get; set; }

        [JsonProperty("esadoSituacion")]
        public int TimeManagementStatus { get; set; }

        [JsonProperty("tieneReporte")]
        public bool HasReport { get; set; }

        [JsonProperty("porcentajeAvance")]
        public string PercentageOfCompletion { get; set; }

        public TTrafficLightStatus TrafficLight
        {
            get
            {
                if (Status != TStatusTasK.Finished && Status != TStatusTasK.FinishedIncomplete && Status != TStatusTasK.Suspended)
                {
                    TimeSpan diff = EndProgrammedDate.Subtract(DateTime.Today);
                    TTrafficLightStatus result = (int)diff.TotalDays switch
                    {
                        int days when days > 2 => TTrafficLightStatus.InProgress,
                        int days when days >= 0 && days <= 2 => TTrafficLightStatus.CloseToDeadline,
                        int days when days < 0 => TTrafficLightStatus.Overdue,
                        _ => TTrafficLightStatus.Completed
                    };
                    return result;
                }
                else
                {
                    return TTrafficLightStatus.Completed;
                }
            }
        }
    }
}
