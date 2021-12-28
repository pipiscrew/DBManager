            string user, password, host, port, service;

            port = "2233";
            host = "server.com";
            service = "srv";
            user = "root";
            password = "password";

            string conn = @"Data Source=(DESCRIPTION=(ADDRESS_LIST="
            + "(ADDRESS=(PROTOCOL=TCP)(HOST=" + host + ")(PORT=" + port + ")))"
            + "(CONNECT_DATA=(SERVICE_NAME=" + service + ")));"
            + "User Id=" + user + ";Password=" + password;

            OracleException x = null;
            var test = new ORACLEClass(conn, out x);

            if (x != null)
            {
                MessageBox.Show(x.Message);
            }
