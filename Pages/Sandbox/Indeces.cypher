CREATE TEXT INDEX paper_text_index IF NOT EXISTS FOR (n:Paper) ON (n.Excerpt)

CREATE TEXT INDEX paper_text_index IF NOT EXISTS FOR (n:Paper) ON (n.Description)

CREATE TEXT INDEX paper_text_index IF NOT EXISTS FOR (n:Paper) ON (n.Content)

// drop index excerptsAndContents
CREATE FULLTEXT INDEX excerptsAndContents FOR (n:Paper|Page) ON EACH [n.Content, n.Excerpt]