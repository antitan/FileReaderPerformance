  logger.LogTrace($" analyse paymentRequest {JsonSerializer.Serialize(paymentRequestActivity, new JsonSerializerOptions { WriteIndented = true })}");

  PaymentWorflowRulesResult paymentWorflowRulesResult = new PaymentWorflowRulesResult();

  var ruleParameters = new[]
  {
      new RuleParameter("ForbiddenIpList", aggregateRootRestrictionItems.ForbiddenIpList),
      new RuleParameter("ForbiddenAccountList", aggregateRootRestrictionItems.ForbiddenAccountList),
      new RuleParameter("ForbiddenCountryList", aggregateRootRestrictionItems.ForbiddenCountryList),
      new RuleParameter("UserDataActivitiesHistory", aggregateRootUserHistory),
      new RuleParameter("PaymentRequestActivity", paymentRequestActivity)
  };

  //execute worflow rules
  var results = await rulesEngine.ExecuteAllRulesAsync(workflow.Name, ruleParameters);

  //persisit the paymentRequestActivity with activity and fingerPrint
  await domainPaymentActivityRepository.SaveAsync(paymentRequestActivity, cancellationToken);
  paymentWorflowRulesResult.ActivityId = paymentRequestActivity.ActivityId;

  bool isChallenged = false;
  foreach (var ruleResult in results)
  {
      var expectedActionTypeId = (ActionTypeEnum)ruleResult.Rule.Properties[EngineRuleProperties.ExpectedActionTypeId];

      logger.LogTrace($" process rule result : {JsonSerializer.Serialize(ruleResult.Rule, new JsonSerializerOptions { WriteIndented = true })}");
      if (ruleResult.IsSuccess)
      {
          if (expectedActionTypeId == ActionTypeEnum.Challenge)
          {
              isChallenged = true;
              //save the firt rule whose failed
              if (paymentWorflowRulesResult.FirstChallengedRule == null)
              {
                  paymentWorflowRulesResult.FirstChallengedRule = new RulesEngine.Models.Rule
                  {
                      RuleName = ruleResult.Rule.RuleName,
                      Expression= ruleResult.Rule.Expression
                  };
              }
              paymentWorflowRulesResult.NeedChallenge = true;

              ActivityWorkflow actWorkflow = new ActivityWorkflow
              {
                  WorkFlowId = (Guid)ruleResult.Rule.Properties[EngineRuleProperties.WorkFlowId],
                  ActivityId = paymentRequestActivity.ActivityId,
                  ActivityType = ActivityTypeEnum.Payment,
                  RuleId = (Guid)ruleResult.Rule.Properties[EngineRuleProperties.Id],
                  ExecutionResult = false,
              };
              await domainActivityRepository.AddActivityWorflowAsync(actWorkflow, cancellationToken);
          }
          else if (expectedActionTypeId == ActionTypeEnum.Allow)
          {
              ActivityWorkflow actWorkflow = new ActivityWorkflow
              {
                  WorkFlowId = (Guid)ruleResult.Rule.Properties[EngineRuleProperties.WorkFlowId],
                  ActivityId = paymentRequestActivity.ActivityId,
                  ActivityType = ActivityTypeEnum.Payment,
                  RuleId = (Guid)ruleResult.Rule.Properties[EngineRuleProperties.Id],
                  ExecutionResult = true,
              };
              await domainActivityRepository.AddActivityWorflowAsync(actWorkflow, cancellationToken);
          }
      }
      //IsSuccess == false
      else
      {
          if (expectedActionTypeId == ActionTypeEnum.Challenge)
          {
              ActivityWorkflow actWorkflow = new ActivityWorkflow
              {
                  WorkFlowId = (Guid)ruleResult.Rule.Properties[EngineRuleProperties.WorkFlowId],
                  ActivityId = paymentRequestActivity.ActivityId,
                  ActivityType = ActivityTypeEnum.Payment,
                  RuleId = (Guid)ruleResult.Rule.Properties[EngineRuleProperties.Id],
                  ExecutionResult = true,
              };
              await domainActivityRepository.AddActivityWorflowAsync(actWorkflow, cancellationToken);
          }
          else if (expectedActionTypeId == ActionTypeEnum.Allow)
          {
              isChallenged = true;
              //save the firt rule whose failed
              if (paymentWorflowRulesResult.FirstChallengedRule == null)
              {
                  paymentWorflowRulesResult.FirstChallengedRule = new RulesEngine.Models.Rule
                  {
                      RuleName = ruleResult.Rule.RuleName,
                      Expression = ruleResult.Rule.Expression
                  };
              }
              paymentWorflowRulesResult.NeedChallenge = true;

              ActivityWorkflow actWorkflow = new ActivityWorkflow
              {
                  WorkFlowId = (Guid)ruleResult.Rule.Properties[EngineRuleProperties.WorkFlowId],
                  ActivityId = paymentRequestActivity.ActivityId,
                  ActivityType = ActivityTypeEnum.Payment,
                  RuleId = (Guid)ruleResult.Rule.Properties[EngineRuleProperties.Id],
                  ExecutionResult = false,
              };
              await domainActivityRepository.AddActivityWorflowAsync(actWorkflow, cancellationToken);
          }
      }
  }

  logger.LogTrace($"Summary of the workflow {workflow.Name} execution with the paymentRequest {JsonSerializer.Serialize(paymentRequestActivity, new JsonSerializerOptions { WriteIndented = true })}");

  if (isChallenged) 
   logger.LogTrace($" the workflow {workflow.Name}  has failed with the rule { JsonSerializer.Serialize( paymentWorflowRulesResult?.FirstChallengedRule, new JsonSerializerOptions { WriteIndented=true}) } ");
  else
      logger.LogTrace($" the workflow {workflow.Name} run successfully");

  //if there is one challenge at least : activity result is false, we ll return scaRequired=true with activityId value
  //Else activity result is true , we ll return scaRequired=false with activityId=null
  await domainActivityRepository.SetActivityResultAsync(paymentRequestActivity.ActivityId, !isChallenged,cancellationToken);

  return Result.Ok(paymentWorflowRulesResult)!;
