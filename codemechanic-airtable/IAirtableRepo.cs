public interface IAirtableRepo {
       Task<List<T>> SearchRecords<T>(AirtableSearch search);
}