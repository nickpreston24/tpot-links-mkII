drop table if exists tpot_papers;
drop table if exists facebook_comments;

CREATE TABLE tpot_papers
(
    id           int not null auto_increment,
    wordpress_id int,
    author       varchar(255),
    category     varchar(255),
    link         varchar(255),
    excerpt      TEXT,
    markdown     TEXT,
    frontmatter  TEXT,
    RawHtml      LONGTEXT,
    RawJson      JSON,

    primary key (id)
);


CREATE TABLE facebook_comments
(
    id          int not null auto_increment,
    CssSelector TEXT,
    RawHtml     LONGTEXT,
    RawJson     JSON,
    primary key (id)
);

insert into tpot_papers
values (1, 2, 10, "blah", "blah", "blah", "blah", "blah", "blah", "{}")
     , (1, 2, 10, "blah", "blah", "blah", "blah", "blah", "blah", "{}")
     , (1, 2, 10, "blah", "blah", "blah", "blah", "blah", "blah", "{}")
     , (1, 2, 10, "blah", "blah", "blah", "blah", "blah", "blah", "{}")
     , (1, 2, 10, "blah", "blah", "blah", "blah", "blah", "blah", "{}")
;


select *
from facebook_comments;

select *
from tpot_papers;




