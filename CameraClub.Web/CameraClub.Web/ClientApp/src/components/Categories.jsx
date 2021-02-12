import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { CategoriesModal } from './CategoriesModal';
import { ClubApi } from '../ClubApi';

export class Categories extends Component {
    clubApi;

    constructor(props) {
        super(props);
        this.state = {
            categoryData: [],
            error: false,
            loading: true,
            errorMessage: "",
            isModalVisible: false
        }

        this.clubApi = new ClubApi();

        this.showModal = this.showModal.bind(this);
        this.hideModal = this.hideModal.bind(this);
        this.loadState = this.loadState.bind(this);
        this.showError = this.showError.bind(this);
        this.translate = this.translate.bind(this);
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

    translate(category) {
        if (category.id !== null) {
            var categoryToUpdate = this.state.categoryData.find(c => c.id === category.id);
            categoryToUpdate.name = category.name;
            categoryToUpdate.isDigital = category.isDigital;
        }
        else {
            this.state.categoryData.push(category);
        }
    }

    loadState(categories) {
        this.setState({
            loading: false,
            error: false,
            errorMessage: null,
            categoryData: categories,
            currentCategory: categories[0]
        });
    }

    handleSave = (competition) => {
        this.clubApi.save("UpsertCategory", competition, this.translate, this.hideModal, this.showError);
    }

    getCategoryData() {
        this.clubApi.load("GetCategories", this.showError, this.loadState);
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