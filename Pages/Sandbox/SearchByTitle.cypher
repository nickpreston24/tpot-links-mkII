MATCH (paper:Paper)
WHERE toLower(paper.Title) contains toLower($Title)
RETURN paper LIMIT 25;