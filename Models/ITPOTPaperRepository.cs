namespace TPOT_Links;

public interface ITPOTPaperRepository
{
	void InsertTPOTPaper(TPOTMarkdownPaper tpotMarkdownPaper);
	IList<TPOTMarkdownPaper> GetTPOTPaperByType(string type);
	void UpdateTPOTPaperList(IList<TPOTMarkdownPaper> TPOTPaperList);
}
