Feature: Apifeature

  Scenario: Validate GET api for status code 200
    When I requested the get /api/v1/employees API
    Then I should get status code 200

    Scenario: Validate POST api for status code 200
    When I requested the post /post API
    Then I should get status code 200

    Scenario: Validate Delete api status code is 200
    When I requested the Delete /api/v1/delete/719 api
    Then I should get status code 200
    
    Scenario: Validate status code 200 of Get API with the help of Token
    When I saved the token from the api /auth/login
    When I saved the response of the Get api /auth/me
    Then I should get status code 200
