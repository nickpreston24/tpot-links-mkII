using AirtableApiClient;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using TPOT_Links;

public interface IAirtableRepo {
       Task<List<T>> SearchRecords<T>(AirtableSearch search);
}