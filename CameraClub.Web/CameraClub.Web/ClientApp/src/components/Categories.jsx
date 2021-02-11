import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { CategoriesModal } from './CategoriesModal';

export class Categories extends Component {
    static displayName = Categories.name;

    constructor(props) {
        super(props);
        this.state = {
            categoryData: [],
            error: false,
            loading: true,
            errorMessage: "",
            isModalVisible: false
        }

        this.showModal = this.showModal.bind(this);
        this.hideModal = this.hideModal.bind(this);
        this.handleSave = this.handleSave.bind(this);
    }

    componentDidMount() {
        this.getCategoryData();
    }

    showModal = (category) => {
        if (category === null) {
            category = { "id": null, "name": "", "isDigital": false };
        }

        this.setState(
            {
                isModalVisible: true,
                currentCategory: category
            });
    };

    hideModal = () => {
        this.setState({ isModalVisible: false });
    };

    showError(error) {
        console.log(error);
        this.setState({
            loading: false,
            error: true,
            errorMessage: error,
            userApprovals: null
        });
    }

    handleSave = (category) => {
        var url = process.env.REACT_APP_API_URL + "UpsertCategory";

        if (category.id !== null) {
            fetch(url,
                {
                    method: "PUT",
                    headers: {
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify({
                        id: category.id,
                        name: category.name,
                        isDigital: category.isDigital
                    })
                })
                .then(
                    (result) => {
                        var categoryToUpdate = this.state.categoryData.find(c => c.id === category.id);
                        categoryToUpdate.name = category.name;
                        categoryToUpdate.isDigital = category.isDigital;

                        this.hideModal();
                    },
                    (error) => {
                        this.hideModal();
                        this.showError(error);
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
                    body: JSON.stringify({
                        name: category.name,
                        isDigital: category.isDigital
                    })
                })
                .then(
                    (result) => {
                        this.state.categoryData.push(category);

                        this.hideModal();
                    },
                    (error) => {
                        this.hideModal();
                        this.showError(error);
                    }
                );
        }
    }

    getCategoryData() {
        var url = process.env.REACT_APP_API_URL + 'GetCategories';

        fetch(url,
            {
                method: "GET"
            })
            .then(response => response.json())
            .then(
                (result) => {
                    this.setState({
                        loading: false,
                        error: false,
                        errorMessage: null,
                        categoryData: result === null ? [] : result,
                        currentCategory: result === null ? [] : result[0]
                    })
                },
                (error) => {
                    this.showError(error);
                }
            );
    }

    renderCategories() {
        return (
            <>
                <Row>
                    <Col>
                        <h1 className="page-title">Categories</h1>
                    </Col>
                </Row>
                <Row>
                    <Col className="text-right">
                        <button className="btn btn-primary" onClick={(e) => { e.preventDefault(); this.showModal(null); }}>Add Category</button>
                    </Col>
                </Row>
                <Row>
                    {this.state.categoryData.map(category =>
                        <Container key={category.id} className="bs-callout bs-callout-info">
                            <Row>
                                <Col>
                                    <h4 className="info">{category.name}</h4>
                                </Col>
                            </Row>
                            <Row className="top-margin-spacing">
                                <Col>Digital category? {category.isDigital ? "Yes" : "No"}</Col>
                            </Row>
                            <Row className="top-margin-spacing">
                                <Col>
                                    <button className="btn btn-link" onClick={(e) => { e.preventDefault(); this.showModal(category); }}>Edit</button>
                                </Col>
                            </Row>
                        </Container>
                    )}
                </Row>
                <CategoriesModal handleClose={this.hideModal} handleSave={this.handleSave} show={this.state.isModalVisible} categoryData={this.state.currentCategory} />
            </>
        );
    }


    render() {
        let contents = this.state.error
            ? <p>Error:  <span dangerouslySetInnerHTML={{ __html: this.state.errorMessage }}></span></p>
            : this.state.loading
                ? <p><em>Loading...</em></p>
                : this.renderCategories();

        return (
            <div>
                {contents}
            </div>
        );
    }
}