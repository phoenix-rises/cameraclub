export const save = (urlAction, data, translate, hideModal, showError) => {
    var url = process.env.REACT_APP_API_URL + urlAction;

    if (data.id !== null) {
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
                    hideModal();
                },
                (error) => {
                    hideModal();
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
                    hideModal();
                },
                (error) => {
                    hideModal();
                    showError(error);
                }
            );
    }
}

export const load = (urlAction, showError, loadState) => {
    var url = process.env.REACT_APP_API_URL + urlAction;

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