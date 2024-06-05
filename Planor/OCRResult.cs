using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planor
{
    /// <summary>
    /// Represents the root object of the OCR response. Contains parsed results, exit code, and error information.
    /// </summary>
    public class Rootobject
    {
        /// <summary>
        /// Gets or sets the parsed results. An array of Parsedresult objects, each representing the parsed result of a single file.
        /// </summary>
        public Parsedresult[] ParsedResults { get; set; }

        /// <summary>
        /// Gets or sets the exit code of the OCR operation. A value indicating whether the OCR operation was successful (0) or not (non-zero).
        /// </summary>
        public int OCRExitCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an error occurred during processing. A boolean value indicating whether an error occurred during processing (true) or not (false).
        /// </summary>
        public bool IsErroredOnProcessing { get; set; }

        /// <summary>
        /// Gets or sets the error message. A string containing a brief description of the error that occurred during processing.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error details. A string containing detailed information about the error that occurred during processing.
        /// </summary>
        public string ErrorDetails { get; set; }
    }

    /// <summary>
    /// Represents the parsed result of a single file. Contains parsed text, exit code, and error information.
    /// </summary>
    public class Parsedresult
    {
        /// <summary>
        /// Gets or sets the exit code of the file parse operation. A value indicating whether the file parse operation was successful (0) or not (non-zero).
        /// </summary>
        public object? FileParseExitCode { get; set; }

        /// <summary>
        /// Gets or sets the parsed text. A string containing the text extracted from the file.
        /// </summary>
        public string ParsedText { get; set; }

        /// <summary>
        /// Gets or sets the error message. A string containing a brief description of the error that occurred during file parsing.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error details. A string containing detailed information about the error that occurred during file parsing.
        /// </summary>
        public string ErrorDetails { get; set; }
    }
}
