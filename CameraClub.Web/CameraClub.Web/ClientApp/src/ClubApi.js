export class ClubApi {
    baseUrl;

    constructor() {
        this.baseUrl = process.env.REACT_APP_API_URL;
    }

    save = (urlAction, data, translate, showError, alwaysUpdate = false) => {
        var url = this.baseUrl + urlAction;

        if (data.id || alwaysUpdate) {
            fetch(url,
                {
                    method: "PUT",
                    headers: {
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify(data)
                })
                .then(
                    (result) => {
                        translate(data);
                    },
                    (error) => {
                        showError(error);
                    }
                );
        }
        else {
            fetch(url,
                {
                    method: "POST",
                    headers: {
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify(data)
                })
                .then(
                    (result) => {
                        translate(data);
                    },
                    (error) => {
                        showError(error);
                    }
                );
        }
    }

    load = (urlAction, showError, loadState) => {
        var url = this.baseUrl + urlAction;

        fetch(url,
            {
                method: "GET"
            })
            .then(response => response.json())
            .then(
                (result) => {
                    loadState(result);
                },
                (error) => {
                    showError(error);
                }
            );
    }

    async saveWithPut(urlAction, data) {
        var url = this.baseUrl + urlAction;

        const response = await fetch(url,
            {
                method: "PUT",
                headers: {
                    'Content-type': 'application/json'
                },
                body: JSON.stringify(data)
            });

        return response;
    }

    async saveFormData(urlAction, data) {
        var url = this.baseUrl + urlAction;

        const response = await fetch(url,
            {
                method: "POST",
                body: data
            });

        return response;
    }
}