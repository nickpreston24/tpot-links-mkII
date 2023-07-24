# Checkup Page

I'm thinking it would be nice to run checkups on existing pages stored in Neo4j

Run sequence:
1. Get a Paper's HTML from Neo4j
2. Pull Paper from WP by slug - ones we know have irregularities, for testing.
   1. Use the `AsHtmlString()` extension you wrote.
3. Load papers as Cards from filesystem.
4. Loading spinner between ea step
5. Return the results as a small table showing:
   1. Count of irregularities
   2. Breakdown of each irregularity:
      1. Types
         1. Difference in count of bold styles
         2. Difference in count of italic styles
         3. Mismatch in count of bold AND italic styles 
   3. Special rules for finding the irregularities:
      1. The HTML generated from the DOCX must roughly match the shape of the HTML from NEO4j
      2. The NEO4j HTML must be EXACTLY the same as the WP HTML.  If it is not, then show an ERROR Alert.