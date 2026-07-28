-- Creates the table backing the refresh token persistence layer.
-- Tokens are opaque random values; validity/rotation state lives entirely in this row.

CREATE TABLE auth.refresh_tokens
(
    refresh_token_id   INT IDENTITY(1,1) NOT NULL,
    user_id            INT NOT NULL,
    token               NVARCHAR(500) NOT NULL,
    expires_at          DATETIME2 NOT NULL,
    created_at          DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    created_by          INT NULL,
    revoked_at          DATETIME2 NULL,
    replaced_by_token   NVARCHAR(500) NULL,
    is_revoked          BIT NOT NULL DEFAULT 0,
    ip_address          NVARCHAR(45) NULL,
    user_agent          NVARCHAR(500) NULL,
    is_enabled          BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_refresh_tokens PRIMARY KEY CLUSTERED (refresh_token_id),
    CONSTRAINT FK_refresh_tokens_users FOREIGN KEY (user_id) REFERENCES auth.users (user_id),
    CONSTRAINT UQ_refresh_tokens_token UNIQUE (token)
);
GO

-- Speeds up the common lookup pattern: "active tokens for this user".
CREATE NONCLUSTERED INDEX IX_refresh_tokens_user_id
    ON auth.refresh_tokens (user_id)
    WHERE is_revoked = 0 AND is_enabled = 1;
GO
