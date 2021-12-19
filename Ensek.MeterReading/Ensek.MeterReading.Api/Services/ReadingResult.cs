using System;
using System.Collections.Generic;


namespace Ensek.MeterReading.Api.Services
{
    public class ValidationResult
    {
        public ValidationResult(bool isValid)
        {
            IsValid = isValid;
        }

        /// <summary>
        /// Indicator of successful validation
        /// </summary>
        public bool IsValid { get; set; }

        
        /// <summary>
        /// Error messages pertaining to validation result to be shown to users/clients
        /// </summary>
        public string ClientMessage { get; set; }

        
        /// <summary>
        /// Any exception caught while processing the validation
        /// </summary>
        public Exception Exception { get; set; }
        
        
    }

    public class ReadingResult
    {
        public List<ValidationResult> ValidationResults { get; set; }
        
        public int TotalSuccessfulReadings { get; set; }
        
        public int TotalFailedReadings { get; set; }
    }
}

