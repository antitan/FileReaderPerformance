ALTER TABLE [dbo].[Activity] ADD  CONSTRAINT [DF_Activity_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[ActivityWorkflow] ADD  CONSTRAINT [DF_ActivityWorkflow_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[ChallengeRequestHistory] ADD  CONSTRAINT [DF_ChallengeRequestHistory_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[ChallengeRequestHistory] ADD  CONSTRAINT [DF__Challenge__Creat__09A971A2]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[FingerPrint] ADD  CONSTRAINT [DF_FingerPrint_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[GeoLocationInfo] ADD  CONSTRAINT [DF_GeoLocationInfo_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[PaymentActivity] ADD  CONSTRAINT [DF_PaymentActivity_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[RestrictionList] ADD  CONSTRAINT [DF_RestrictionList_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[RestrictionValue] ADD  CONSTRAINT [DF_RestrictionValue_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Rule] ADD  CONSTRAINT [DF_Rule_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Workflow] ADD  CONSTRAINT [DF_Workflow_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD  CONSTRAINT [FK_Activity_ActivityType] FOREIGN KEY([ActivityTypeId])
REFERENCES [dbo].[ActivityType] ([Id])
GO
ALTER TABLE [dbo].[Activity] CHECK CONSTRAINT [FK_Activity_ActivityType]
GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD  CONSTRAINT [FK_Activity_FingerPrint] FOREIGN KEY([FingerPrintId])
REFERENCES [dbo].[FingerPrint] ([Id])
GO
ALTER TABLE [dbo].[Activity] CHECK CONSTRAINT [FK_Activity_FingerPrint]
GO
ALTER TABLE [dbo].[ActivityWorkflow]  WITH CHECK ADD  CONSTRAINT [FK_ActivityWorkflow_Rule] FOREIGN KEY([RuleId])
REFERENCES [dbo].[Rule] ([Id])
GO
ALTER TABLE [dbo].[ActivityWorkflow] CHECK CONSTRAINT [FK_ActivityWorkflow_Rule]
GO
ALTER TABLE [dbo].[ActivityWorkflow]  WITH CHECK ADD  CONSTRAINT [FK_ActivityWorkflow_Workflow] FOREIGN KEY([WorkFlowId])
REFERENCES [dbo].[Workflow] ([Id])
GO
ALTER TABLE [dbo].[ActivityWorkflow] CHECK CONSTRAINT [FK_ActivityWorkflow_Workflow]
GO
ALTER TABLE [dbo].[ChallengeRequestHistory]  WITH CHECK ADD  CONSTRAINT [FK_ChallengeRequestHistory_Activity_ActivityType] FOREIGN KEY([ActivityId], [ActivityTypeId])
REFERENCES [dbo].[Activity] ([Id], [ActivityTypeId])
GO
ALTER TABLE [dbo].[ChallengeRequestHistory] CHECK CONSTRAINT [FK_ChallengeRequestHistory_Activity_ActivityType]
GO
ALTER TABLE [dbo].[GeoLocationInfo]  WITH CHECK ADD  CONSTRAINT [FK_GeoLocationInfo_FingerPrint] FOREIGN KEY([FingerPrintId])
REFERENCES [dbo].[FingerPrint] ([Id])
GO
ALTER TABLE [dbo].[GeoLocationInfo] CHECK CONSTRAINT [FK_GeoLocationInfo_FingerPrint]
GO
ALTER TABLE [dbo].[PaymentActivity]  WITH CHECK ADD  CONSTRAINT [FK_PaymentActivity_Activity_ActivityType] FOREIGN KEY([ActivityId], [ActivityTypeId])
REFERENCES [dbo].[Activity] ([Id], [ActivityTypeId])
GO
ALTER TABLE [dbo].[PaymentActivity] CHECK CONSTRAINT [FK_PaymentActivity_Activity_ActivityType]
GO
ALTER TABLE [dbo].[RestrictionValue]  WITH CHECK ADD  CONSTRAINT [FK_RestrictionValue_RestrictionList] FOREIGN KEY([RestrictionListId])
REFERENCES [dbo].[RestrictionList] ([Id])
GO
ALTER TABLE [dbo].[RestrictionValue] CHECK CONSTRAINT [FK_RestrictionValue_RestrictionList]
GO
ALTER TABLE [dbo].[Rule]  WITH CHECK ADD  CONSTRAINT [FK_Rule_ActionType] FOREIGN KEY([ExpectedActionTypeId])
REFERENCES [dbo].[ActionType] ([Id])
GO
ALTER TABLE [dbo].[Rule] CHECK CONSTRAINT [FK_Rule_ActionType]
GO
ALTER TABLE [dbo].[Rule]  WITH CHECK ADD  CONSTRAINT [FK_Rule_Workflow] FOREIGN KEY([WorkFlowId])
REFERENCES [dbo].[Workflow] ([Id])
GO
ALTER TABLE [dbo].[Rule] CHECK CONSTRAINT [FK_Rule_Workflow]
GO
ALTER TABLE [dbo].[Workflow]  WITH CHECK ADD  CONSTRAINT [FK_Workflow_ActivityType] FOREIGN KEY([ActivityTypeId])
REFERENCES [dbo].[ActivityType] ([Id])
GO
ALTER TABLE [dbo].[Workflow] CHECK CONSTRAINT [FK_Workflow_ActivityType]
GO
ALTER TABLE [dbo].[PaymentActivity]  WITH CHECK ADD  CONSTRAINT [CheckPaymentActivityValue] CHECK  (([ActivityTypeId]=(1)))
GO
ALTER TABLE [dbo].[PaymentActivity] CHECK CONSTRAINT [CheckPaymentActivityValue]
GO
