CALL db.index.fulltext.queryNodes("excerptsAndContents", $term) YIELD node, score
RETURN node.Excerpt, node.Content, score