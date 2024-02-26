WITH ["257219", "24769", "24935"] AS subs
MATCH (paper:Paper)
WHERE any(word IN subs WHERE paper.Id CONTAINS word)
RETURN paper