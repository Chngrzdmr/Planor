using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planor
{
    /// <summary>
    /// Represents the root object of the OCR response.
    /// </summary>
    public class Rootobject
    {
        /// <summary>
        /// Gets or sets the parsed results.
        /// </summary>
        public public Parsedresult[] ParsedResults { get; set; }

        /// <summary>
        /// Gets or sets the exit code of the OCR operation.
        /// </summary>
        public public int OCRExitCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an error occurred during processing.
        /// </summary>
        public public bool IsErroredOnProcessing { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error details.
        /// </summary>
        public public string ErrorDetails { get; set; }
    }

    /// <summary>
    /// Represents the parsed result of a single file.
    /// </summary>
    public class Parsedresult
    {
        /// <summary>
        /// Gets or sets the exit code of the file parse operation.
        /// </summary>
        public public object? FileParseExitCode { get; set; }

        /// <summary>
        /// Gets or sets the parsed text.
        /// </summary>
        public public string ParsedText { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error details.
        /// </summary>
        public public string ErrorDetails { get; set; }
    }
}
