namespace SigobMobile.Common.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Participant of Instruction Model
    /// </summary>
    public class InstructionParticipant
    {
        [JsonProperty("codigoDespacho")]
        public string OfficeId { get; set; }

        [JsonProperty("nombre")]
        public string FullName { get; set; }

        [JsonProperty("cargo")]
        public string Position { get; set; }
    }

    /// <summary>
    /// Instruction Model
    /// </summary>
    public class Instruction
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("idCentroGestion")]
        public int ManagementCenterId { get; set; }

        [JsonProperty("centroGestion")]
        public string ManagementCenterName { get; set; }

        [JsonProperty("nombreInstruccion")]
        public string Title { get; set; }

        [JsonProperty("detalleInstruccion")]
        public string Description { get; set; }

        [JsonProperty("fechaTermino")]
        public DateTimeOffset EndDate { get; set; }

        [JsonProperty("despachoProgramador")]
        public string ProgrammerId { get; set; }

        [JsonProperty("nombreProgramador")]
        public string ProgrammerFullName { get; set; }

        [JsonProperty("cargoProgramador")]
        public string ProgrammerPosition { get; set; }

        [JsonProperty("plazoVencimiento")]
        public int ExpiryPeriod { get; set; }

        [JsonProperty("cantidadTareas")]
        public int TasksAmount { get; set; }

        [JsonProperty("cantidadDocumentos")]
        public int DocumentsAmount { get; set; }

        [JsonProperty("cantidadParticipantes")]
        public int ParticipantsAmount { get; set; }

        [JsonProperty("finalizada")]
        public byte IsFinished { get; set; }

        [JsonProperty("isModifiable")]
        public bool IsModifiable { get; set; }

        [JsonProperty("canBeFinished")]
        public bool CanBeFinished { get; set; }

        [JsonProperty("participantes")]
        public List<InstructionParticipant> Participants { get; set; }
    }
}
