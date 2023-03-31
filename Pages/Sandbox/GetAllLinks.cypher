MATCH page=()-[:LINKS_TO]->() 
RETURN page LIMIT $limit;