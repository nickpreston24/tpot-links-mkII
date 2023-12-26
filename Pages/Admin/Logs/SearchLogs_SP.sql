drop table if exists logs;
create table logs
(
    id               int NOT NULL AUTO_INCREMENT, -- not very critical, but nice for tracking how many rows and what we've removed.
# WHAT happened?
    exception_text   text,                        -- stack traces, exception messages, etc.
    sql_parameters   json,                        -- Records what params were passed so we can figure out what went wrong.
    payload          json,                        -- what JSON if any was passed thru the API on the way to our CRUD operation?
    diff             json,                        -- a JsonDiff of what changed in the record (if anything).
    operation_name   varchar(250),                -- e.g. a CRUD operation, Stored Proc, or a cron, etc. This, plus the app version and name helps us understand what process failed regardless of version.

# Where did it happen?
    breadcrumb       varchar(250) default '',
    table_name       varchar(250),
    server_name      varchar(250),
    database_name    varchar(250),

# Who is responsible?
    application_name varchar(250),                -- or can be 'Application Id'.
    modified_at      datetime,
    created_at       datetime,
    modified_by      varchar(250),
    created_by       varchar(250),

# How can we resolve it?
    commit_url       text,                        -- the commit url on Github, Gitlab, bitbucket, etc.  (death to tortoise svn!)
    issue_url        text,                        -- Hopefully not JIRA...but, if we must...

# Meta/Management
    is_deleted       bit          default 0,      -- soft delete
    is_archived      bit          default 0,      -- if on, no-touchy (don't delete it)!
    is_enabled       bit          default 0,      -- soft hide

# PK's
    PRIMARY KEY (id)

);

insert into logs ( table_name
                 , database_name
                 , exception_text
                 , breadcrumb
                 , issue_url
                 , created_by
                 , modified_by
                 , created_at
                 , modified_at
                 , is_deleted
                 , is_archived
                 , is_enabled)
values ( 'tpotpapers'
       , 'railway'
       , 'Null Reference Exception ... '
       , 'Home > XYZ > ABC'
       , 'jira.com/blah...'
       , 'Braden Preston'
       , 'Nick Preston'
       , now()
       , now()
       , 0
       , 1
       , 0),
       ( 'tpotpapers'
       , 'railway'
       , 'Argument Missing Exception ... '
       , 'Home > Sandbox > ABC'
       , 'jira.com/blah...'
       , 'Nick Preston'
       , 'Braden Preston'
       , now()
       , now()
       , 1
       , 0
       , 0),
       ( 'tpotpapers'
       , 'railway'
       , 'Argument Missing Exception ... '
       , 'Home > Sandbox > ABC'
       , 'jira.com/blah...'
       , 'Alan Agnew'
       , 'Braden Preston'
       , now()
       , now()
       , 0
       , 0
       , 1)
;

/* Your Logs API */

# Search by filters
drop procedure if exists SearchLogs;
DELIMITER ^_^

CREATE PROCEDURE SearchLogs(
    created_by varchar(250),
    is_archived bit,
    is_deleted bit,
    is_enabled bit
)
BEGIN
    #     select is_archived, is_deleted, is_enabled as inputs;
    SELECT *
    FROM logs logrow
    where 1 = 1
        # text filters
        AND (
              created_by is not null and logrow.created_by = created_by
              )
       # flags
       OR (
                logrow.is_archived = is_archived
            AND
                logrow.is_deleted = is_deleted
            AND
                logrow.is_enabled = is_enabled
        );
END ^_^

DELIMITER ;
/* Test our optional params and specific users
   */
-- nothing is right, no rows.
call SearchLogs(null, 0, 0, 0);
-- wrong name, right flag.
call SearchLogs('Nick', 1, null, null);
-- right name, ignore flags.
call SearchLogs('Nick Preston', null, null, null);

/* Test our flag filters */
-- archived only
call SearchLogs(null, 1, 0, 0);
-- deleted only
call SearchLogs(null, 0, 1, 0);
-- enabled only
call SearchLogs(null, 0, 0, 1);

-- it will never be all 3
call SearchLogs(null, 1, 1, 1);
