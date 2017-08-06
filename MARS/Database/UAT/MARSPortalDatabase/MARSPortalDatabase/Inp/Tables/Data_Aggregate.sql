CREATE TABLE [Inp].[Data_Aggregate] (
    [Type]                INT      NOT NULL,
    [PeriodStart]         DATETIME NOT NULL,
    [PeriodEnd]           DATETIME NOT NULL,
    [min_dim_Calendar_id] INT      NOT NULL,
    [max_dim_Calendar_id] INT      NOT NULL,
    [z_inserted]          DATETIME DEFAULT (getdate()) NOT NULL,
    [z_updated]           DATETIME DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [pk_Data_Aggregate] PRIMARY KEY CLUSTERED ([Type] ASC, [PeriodStart] ASC)
);


GO
create trigger inp.tr_Data_Aggregate on inp.Data_Aggregate for insert, update
as
	if exists	(	select	*
					from	inp.Data_Aggregate t1
						join inp.Data_Aggregate t2
							on t1.Type = t2.Type
							and	t1.PeriodStart <= t2.PeriodEnd
							and t1.PeriodEnd >= t2.PeriodStart
					where t1.PeriodStart <> t2.PeriodStart
				)
	begin
			raiserror ('overlapping periods', 16, -1)
			rollback tran
	end