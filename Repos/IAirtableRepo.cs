using AirtableApiClient;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeMechanic.RazorPages;
public interface IAirtableRepo {
       Task<List<T>> ListRecords<T>(AirtableSearch search);
}