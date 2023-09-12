public interface IAirtableRepo {
       Task<List<T>> SearchRecords<T>(AirtableSearch search, bool debug_mode = false);
}