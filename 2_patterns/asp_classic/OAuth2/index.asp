
<!-- #include File="aspJSON1.19.asp" -->

<!--
https://www.aspjson.com/
https://github.com/gerritvankuipers/aspjson
-->

<%
Session.Timeout=300 'minutes - 300 / 60 = 5h -- https://www.w3schools.com/asp/asp_sessions.asp

''''''''''''''''''''''''''''''''''''
' PUBLIC VARIABLE STORE THE TOKEN
''''''''''''''''''''''''''''''''''''
Dim accessToken

''''''''''''''''''''''''''''''''''''
' FUNCTION TO GET THE TOKEN
''''''''''''''''''''''''''''''''''''
function GetAccessToken()
    'or else through JS - https://oauth.net/code/javascript/
    Dim clientID, clientSecret, tokenURL
    clientID = "your_client_id"
    clientSecret = "your_client_secret"
    tokenURL = "https://your_server/token"  

    Dim xmlhttp, postData
    Set xmlhttp = Server.CreateObject("MSXML2.ServerXMLHTTP.6.0")
   
    postData = "grant_type=client_credentials"
    postData = postData & "&client_id=" & Server.URLEncode(clientID)
    postData = postData & "&client_secret=" & Server.URLEncode(clientSecret)

    xmlhttp.Open "POST", tokenURL, False
    xmlhttp.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
    xmlhttp.send postData

    If xmlhttp.Status = 200 Then
        Dim jsonResponse
        jsonResponse = xmlhttp.responseText
        'Response.Write jsonResponse
        'use of external asp for JSON [start]
        Set oJSON = New aspJSON
        oJSON.loadJSON(jsonResponse)
       
        GetAccessToken = oJSON.data("access_token")
       
        Set oJSON = Nothing
        'use of external asp for JSON [end]
    Else
        Response.Write("Error: " & xmlhttp.Status & " - " & xmlhttp.statusText)
    End If

    Set xmlhttp = Nothing

end function

''''''''''''''''''''''''''''''''''''''''''
' SAMPLE API METHOD CALLED WITH THE TOKEN
''''''''''''''''''''''''''''''''''''''''''
function GetClients()
    Dim apiURL, apiResponse
    apiURL = "https://your_server/api/GetClients"

    Set xmlhttp = Server.CreateObject("MSXML2.ServerXMLHTTP.6.0")
    xmlhttp.Open "POST", apiURL, False
    xmlhttp.setRequestHeader "Authorization", "Bearer " & accessToken
    xmlhttp.setRequestHeader "Content-Type", "application/json"
    xmlhttp.setRequestHeader "X-Requested-With", "XMLHttpRequest"
    'xmlhttp.setRequestHeader "Accept", "application/json"

    '## xmlhttp.send()
    '## or
    '## send with body
    xmlhttp.send("{     ""x"": {         ""y"": ""b54d11ad-1a8a-4627-90d8-595ca5f95ae5"",         ""z"": ""6270b145-4e2b-4a58-9e12-d403cafba9ed"",         ""q"": """"     },     ""o"": {        ""Language"":""EN""     } }")

    If xmlhttp.Status = 200 Then
        apiResponse = xmlhttp.responseText
        GetClients = apiResponse
    Else
        Response.Write "API Error: " & xmlhttp.Status & " - " & xmlhttp.statusText
    End If

    Set xmlhttp = Nothing

    GetClients = apiResponse
end function

''''''''''''''''''''''''''''''''''''
' ENTRY POINT
''''''''''''''''''''''''''''''''''''
If Not IsEmpty(Session("accessToken")) Then
    Response.Write "use of session access token"
    accessToken = Session("accessToken")
else
    Response.Write "retrieve new access token"
    accessToken = GetAccessToken()
    Session("accessToken") = accessToken
end if

Dim callAPI

callAPI = GetClients()

Response.Write "<pre>" & callAPI & "</pre>"

%>