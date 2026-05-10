IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [FullName] nvarchar(max) NOT NULL,
    [Major] nvarchar(max) NOT NULL,
    [AvatarUrl] nvarchar(max) NOT NULL,
    [isActive] int NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [Attendances] (
    [id_attendance] uniqueidentifier NOT NULL,
    [id_plan] uniqueidentifier NOT NULL,
    [id_user] nvarchar(max) NOT NULL,
    [status] nvarchar(max) NOT NULL,
    [checkedIn] bit NOT NULL,
    [isActive] int NOT NULL,
    CONSTRAINT [PK_Attendances] PRIMARY KEY ([id_attendance])
);

CREATE TABLE [ParcheMembers] (
    [id_parche_member] uniqueidentifier NOT NULL,
    [id_parche] uniqueidentifier NOT NULL,
    [id_user] nvarchar(max) NOT NULL,
    [role] nvarchar(max) NOT NULL,
    [isActive] int NOT NULL,
    CONSTRAINT [PK_ParcheMembers] PRIMARY KEY ([id_parche_member])
);

CREATE TABLE [Parches] (
    [id_parche] uniqueidentifier NOT NULL,
    [name] nvarchar(max) NOT NULL,
    [description] nvarchar(max) NOT NULL,
    [coverUrl] nvarchar(max) NOT NULL,
    [inviteCode] nvarchar(max) NOT NULL,
    [createdAt] datetime2 NOT NULL,
    [isActive] int NOT NULL,
    CONSTRAINT [PK_Parches] PRIMARY KEY ([id_parche])
);

CREATE TABLE [PlanOptions] (
    [id_plan_option] uniqueidentifier NOT NULL,
    [id_plan] uniqueidentifier NOT NULL,
    [place] nvarchar(max) NOT NULL,
    [time] nvarchar(max) NOT NULL,
    [votesCount] int NOT NULL,
    [isActive] int NOT NULL,
    CONSTRAINT [PK_PlanOptions] PRIMARY KEY ([id_plan_option])
);

CREATE TABLE [Plans] (
    [id_plan] uniqueidentifier NOT NULL,
    [id_parche] uniqueidentifier NOT NULL,
    [title] nvarchar(max) NOT NULL,
    [description] nvarchar(max) NOT NULL,
    [dateWindow_start] datetime2 NOT NULL,
    [dateWindow_end] datetime2 NOT NULL,
    [state] nvarchar(max) NOT NULL,
    [winningOptionId] uniqueidentifier NULL,
    [createdBy] nvarchar(max) NOT NULL,
    [createdAt] datetime2 NOT NULL,
    [isActive] int NOT NULL,
    CONSTRAINT [PK_Plans] PRIMARY KEY ([id_plan])
);

CREATE TABLE [Votes] (
    [id_vote] uniqueidentifier NOT NULL,
    [id_plan] uniqueidentifier NOT NULL,
    [id_user] nvarchar(max) NOT NULL,
    [id_option] uniqueidentifier NOT NULL,
    [isActive] int NOT NULL,
    CONSTRAINT [PK_Votes] PRIMARY KEY ([id_vote])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260510190236_InitialCreate', N'10.0.7');

COMMIT;
GO

