MATCH (page:Page)
WHERE toLower(page.Title) contains toLower($Title)
RETURN page LIMIT 25;   